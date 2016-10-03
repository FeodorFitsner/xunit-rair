using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Rair.Utilities.Core;

namespace Rair.Utilities.Windows.Extensions
{
    public static class FileInfoExtensions
    {
        public static void StreamAction(this FileInfo file, FileMode mode, Action<FileStream> action)
        {
            using (var fs = new FileStream(file.FullName, mode))
            {
                action(fs);
            }
        }

        public static void Create(this FileInfo file, string contents)
        {
            using (var fs = new StreamWriter(file.FullName))
            {
                fs.Write(contents);
            }
        }

        public static bool IsLocked(this FileInfo file)
        {
            FileStream stream = null;

            try
            {
                if (!file.Exists) return false;
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException ex)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                Debug.WriteLine(ex);
                return true;
            }
            finally
            {
                stream?.Close();
            }

            //file is not locked
            return false;
        }

        public static bool Missing(this FileInfo file)
        {
            return !file.Exists;
        }

        public static Result CopyTo(this FileInfo source, FileInfo target, bool overwrite = true)
        {
            try
            {
                var copied = source.CopyTo(target.FullName, overwrite);
                return Result.Success(copied);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }

        public static void Open(this FileInfo file)
        {
            Process.Start(file.FullName);
        }

        public static Result DeserializeXmlFile<T>(this FileInfo file) where T : class
        {
            if (!file.Exists()) return Result.Missing();

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var fileStream = new FileStream(file.FullName, FileMode.Open))
                {
                    var reader = XmlReader.Create(fileStream);
                    var obj = serializer.Deserialize(reader) as T;
                    return Result.Success(obj);
                }
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }

        public static Result SerializeXmlFile<T>(this FileInfo file, T t, bool includeLinebreaks = true) where T : class
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                file.Remove();
                using (var fileStream = new FileStream(file.FullName, FileMode.Create))
                {
                    XmlWriter writer;
                    if (includeLinebreaks)
                    {
                        var settings = new XmlWriterSettings { Indent = true };
                        writer = XmlWriter.Create(fileStream, settings);
                    }
                    else
                    {
                        writer = XmlWriter.Create(fileStream);
                    }
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    serializer.Serialize(writer, t, ns);
                    return Result.Success();
                }
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }
        public static bool Exists(this FileInfo file)
        {
            return File.Exists(file.FullName);
        }

        public static Result Remove(this FileInfo file)
        {
            if (!file.Exists()) return Result.Nothing("File missing");

            try
            {
                file.Delete();
                return !file.Exists() ? Result.Success("File deleted successfully") : Result.Failure("Unable to delete file");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }
        public static string ReadToEnd(this FileInfo file)
        {
            if (!file.Exists()) return String.Empty;
            var fileData = "";
            using (var reader = new StreamReader(file.FullName))
            {
                fileData = reader.ReadToEnd();
            }
            return fileData;
        }

        public static Result WriteAll(this FileInfo file, string data)
        {
            try
            {
                file.Remove();
                using (var writer = new StreamWriter(file.FullName))
                {
                    writer.Write(data);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }

        public static Result Move(this FileInfo file, FileInfo destination, bool overwrite = true)
        {
            try
            {
                if (!file.Exists()) return Result.Failure($"{file} does not exist.");
                if (!overwrite && destination.Exists()) return Result.Nothing($"{destination} already exists");
                if (overwrite && destination.Exists())
                {
                    var del = destination.Remove();
                    if (del.IsFailure) return del;
                }
                file.MoveTo(destination.FullName);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }
    }
}
