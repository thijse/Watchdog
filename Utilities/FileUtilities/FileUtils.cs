using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Utilities
{
    public partial class FileUtils
    {

        public static bool MatchesFolder(string folderName, string folderWildcard, bool caseSensitive = false)
        {
            folderName = Path.GetDirectoryName(folderName);
            if (string.IsNullOrEmpty(folderName)) return false;
            var wildcard = new Wildcard(folderWildcard, caseSensitive);
            return (wildcard.IsMatch(folderName));
        }

        public static string GetRunningFolder()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static bool MatchesFolder(string folderName, string[] folderWildcards, bool caseSensitive = false)
        {
            return folderWildcards.Any(folder => MatchesFolder(folderName, folder, caseSensitive));
        }

        public static bool MatchesFile(string fileName, string fileWildcard, bool caseSensitive = false)
        {
            fileName = Path.GetFileName(fileName);
            if (string.IsNullOrEmpty(fileName)) return false;
            var wildcard = new Wildcard(fileWildcard, caseSensitive);
            return (wildcard.IsMatch(fileName));
        }
        public static List<string> MatchesFilesInFolder(string folderName, string[] fileWildcards, bool caseSensitive = false)
        {
            var fileList = new List<string>();
            if (!Directory.Exists(folderName)) return fileList;
            fileList.AddRange(Directory.GetFiles(folderName).Where(fileName => MatchesFile(fileName, fileWildcards, caseSensitive)));
            return fileList;
        }

        public static bool MatchesFile(string fileName, string[] fileWildcards, bool caseSensitive = false)
        {
            return (fileWildcards.Any(file => MatchesFile(fileName, file, caseSensitive)));
        }

        public static string GetLastPath(string sourcePath)
        {
            var fullPath = Path.GetFullPath(sourcePath).TrimEnd(Path.DirectorySeparatorChar);
            return fullPath.Split(Path.DirectorySeparatorChar).Last();
        }

        public static string ChangeFileFolder(string sourcePath, string destinationFolder)
        {
            return Combine(destinationFolder, Path.GetFileName(sourcePath));
        }

        public static string ChangeFileFolder(string sourcePath, string sourceBase, string destinationFolder)
        {
            var sourcePathAbs = Path.GetFullPath(sourcePath);
            var sourceBaseAbs = Path.GetFullPath(sourceBase);

            if (sourcePathAbs.Substring(0, sourceBaseAbs.Length) != sourceBaseAbs) return null;

            var temp = sourcePathAbs.Remove(0, sourceBaseAbs.Length);
            var destPath = Combine(destinationFolder, temp);
            return destPath;
        }

        public static bool CreateDirectoryForFile(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (dir==null) return false;
            try
            {
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            }
            catch { return false; }
            return true;

        }

        /// <summary>
        /// There are a few interesting combinations of what will and won't combine correctly for Path.Combine, 
        /// and the MSDN page for Path.Combine explains some of these. There's one condition it won't cater for 
        /// properly though - if the 2nd parameter contains a leading '\', for instance '\file.txt', the final 
        /// result will ignore the first parameter. The output of Path.Combine("c:\directory", "\file.txt") is \file.txt'. 
        /// This is not the case if the 1st parameter contains a trailing '\'. 
        ///
        ///string part1 = @"c:\directory";
        ///string part2 = @"file.txt";
        ///
        ///(1) Console.WriteLine(Path.Combine(part1, part2));
        ///(2) Console.WriteLine(Path.Combine(part1 + @"\", part2));
        ///(3) Console.WriteLine(Path.Combine(part1 + @"\", @"\" + part2));
        ///(4) Console.WriteLine(Path.Combine(part1, @"\" + part2));
        ///
        ///The output of this is
        ///
        ///(1) c:\directory\file.txt
        ///(2) c:\directory\file.txt
        ///(3) \file.txt
        ///(4) \file.txt
        ///
        /// The FileUtils.Combine function will give the output
        ///
        ///(1) c:\directory\file.txt
        ///(2) c:\directory\file.txt
        ///(3) c:\directory\file.txt
        ///(4) c:\directory\file.txt 
        /// </summary>
        public static string Combine(string path1, string path2)
        {
            if (string.IsNullOrEmpty(path2)) return path1;
            var a = path1;
            var b = path2.Substring(0, 1) == @"\" ? path2.Substring(1, path2.Length - 1) : path2;
            var c = Path.Combine(a, b);
            return Path.Combine(path1, path2.Substring(0, 1) == @"\" ? path2.Substring(1, path2.Length - 1) : path2);
        }

        public static string Combine(string path1, string path2, string path3)
        {
            return Combine(new List<string>  {path1,path2,path3});
        }

        public static string Combine(string path1, string path2, string path3, string path4)
        {
            return Combine(new List<string>  {path1,path2,path3, path4});
        }

        public static string Combine(string[] paths)
        {
            return Combine(paths.ToList());
        }
        public static string Combine(List<string> paths)
        {
            if (paths.Count == 0) return "";
            if (paths.Count == 1) return paths[0];
            var combinedPath = Combine(paths[0],paths[1]);
            if (paths.Count == 2) return combinedPath;
            
            var pathsList = new List<string> {combinedPath};
            for (var i = 2; i < paths.Count; i++)
            {
                pathsList.Add(paths[i]);
            }
            return Combine(pathsList);


            //return paths.Aggregate(combinedPath, Combine);
        }

        public static string GetBaseName(string path)
        {
            var fullPath = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar);
            var baseName = Path.GetFileName(fullPath);
            return baseName;
        }


        public static string[] GetDirectories(string path)
        {
            path = path.Substring(Path.GetPathRoot(path).Length);
            var pathSeparators = new string[] { "\\" };
            var dirs = path.Split(pathSeparators, StringSplitOptions.RemoveEmptyEntries);
            return dirs;
        }

        public static void DeleteDirectoryContents(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            foreach (var file in dirInfo.GetFiles()) file.Delete();
            foreach (var subDirectory in dirInfo.GetDirectories()) subDirectory.Delete(true);
        }


        public static string RemoveDrive(string path)
        {
            var newPath = "";
            if ( ( path.IndexOf (":", StringComparison.Ordinal) == 1 ) || ( path.IndexOf ( "\\\\", StringComparison.Ordinal ) == 0 ) ) { newPath = path.Substring (2); }
            if ( newPath.IndexOf ( "\\", StringComparison.Ordinal ) > 0 ) newPath = newPath.Substring ( newPath.IndexOf ( "\\", StringComparison.Ordinal ) );
            return newPath;
        }



        public static string GetAbsolutePath(string relativePath, string basePath)
        {
            if (relativePath == null) return null;
            basePath = basePath == null ? Path.GetFullPath(".") : GetAbsolutePath(basePath, null);
            string path;
            // specific for windows paths starting on \ - they need the drive added to them.
            if (!Path.IsPathRooted(relativePath) || "\\".Equals(Path.GetPathRoot(relativePath)))
            {
                var basePathRoot = Path.GetPathRoot(basePath)??"";
                path = relativePath.StartsWith(Path.DirectorySeparatorChar.ToString()) ? Path.Combine(basePathRoot, relativePath.TrimStart(Path.DirectorySeparatorChar)) : Path.Combine(basePath, relativePath);
            }
            else
                path = relativePath;
            // resolves any internal "..\" to get the true full Path.
            return Path.GetFullPath(path);
        }

        public enum RecurseType
        {
            Files,
            Directories,
            FilesDirectories
        }

        public static System.Collections.Generic.IEnumerable<string> RecurseFilesInDirectories(string root, RecurseType recurseType = RecurseType.Files)
        {
            // Data structure to hold names of sub-folders to be
            // examined for files.
            Stack<string> dirs = new Stack<string>(20);

            if (!System.IO.Directory.Exists(root))
            {
                yield break;
               // throw new ArgumentException();
            }
            dirs.Push(root);

            while (dirs.Count > 0)
            {
                var currentDir = dirs.Pop();
                if (recurseType == RecurseType.Directories || recurseType == RecurseType.FilesDirectories)  
                    yield return currentDir;       

                string[] subDirs;
                try
                {
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                }
                    // An UnauthorizedAccessException exception will be thrown if we do not have
                    // discovery permission on a folder or file. 
                    // to ignore the exception and continue enumerating the remaining files and 
                    // folders. It is also possible (but unlikely) that a DirectoryNotFound exception 
                    // will be raised. This will happen if currentDir has been deleted by
                    // another application or thread after our call to Directory.Exists. The 
                catch (UnauthorizedAccessException )
                {
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException )
                {
                    continue;
                }

                string[] files = null;
                try
                {
                    files = System.IO.Directory.GetFiles(currentDir);
                }

                catch (UnauthorizedAccessException)
                {

                    //Console.WriteLine(e.Message);
                    continue;
                }

                catch (System.IO.DirectoryNotFoundException )
                {
                    //Console.WriteLine(e.Message);
                    continue;
                }

                if (recurseType == RecurseType.Files || recurseType == RecurseType.FilesDirectories)
                    foreach (var file in files)
                    {
                        yield return file;
                    }

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (var subdir in subDirs) dirs.Push(subdir);
            }
        }


        // <summary>
        /// Returns true if <paramref name="path"/> starts with the path <paramref name="baseDirPath"/>.
        /// The comparison is case-insensitive, handles / and \ slashes as folder separators and
        /// only matches if the base dir folder name is matched exactly ("c:\foobar\file.txt" is not a sub path of "c:\foo").
        /// </summary>
        public static bool IsSubPathOf(string path, string baseDirPath)
        {
            string normalizedPath = Path.GetFullPath(path.Replace('/', '\\')
                .WithEnding("\\"));

            string normalizedBaseDirPath = Path.GetFullPath(baseDirPath.Replace('/', '\\')
                .WithEnding("\\"));

            return normalizedPath.StartsWith(normalizedBaseDirPath, StringComparison.OrdinalIgnoreCase);
        }

        public static bool DirectoryContainsFiles(string directory)
        {
            return System.IO.Directory.GetFiles(directory).Length > 0;
        }
        public static string GetRelativePath(string absolutePath, string basePath)
        {
            try
            {
                absolutePath = Path.GetFullPath(absolutePath);
                basePath     = Path.GetFullPath(basePath);
            var toUri = new Uri(absolutePath);
            var fromUri = new Uri(basePath);

            var relativeUri = fromUri.MakeRelativeUri(toUri);
            var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath.Replace('/', Path.DirectorySeparatorChar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static string GetParentFolder(string currentpath, int noOfLevels=1)
        {
            currentpath = Path.GetDirectoryName(currentpath);
             var path = currentpath;
             for(var i=0; i< noOfLevels; i++)
             {
                 path = Combine(path,@"..\");
             }
             return (path != null)?Path.GetFullPath(path):"";
        }


        
        /// <summary>
        /// Shortens a file path to the specified length
        /// </summary>
        /// <param name="path">The file path to shorten</param>
        /// <param name="maxLength">The max length of the output path (including the ellipsis if inserted)</param>
        /// <returns>The path with some of the middle directory paths replaced with an ellipsis (or the entire path if it is already shorter than maxLength)</returns>
        /// <remarks>
        /// Shortens the path by removing some of the "middle directories" in the path and inserting an ellipsis. If the filename and root path (drive letter or UNC server name)     in itself exceeds the maxLength, the filename will be cut to fit.
        /// UNC-paths and relative paths are also supported.
        /// The inserted ellipsis is not a true ellipsis char, but a string of three dots.
        /// </remarks>
        /// <example>
        /// ShortenPath(@"c:\websites\myproject\www_myproj\App_Data\themegafile.txt", 50)
        /// Result: "c:\websites\myproject\...\App_Data\themegafile.txt"
        /// 
        /// ShortenPath(@"c:\websites\myproject\www_myproj\App_Data\theextremelylongfilename_morelength.txt", 30)
        /// Result: "c:\...gfilename_morelength.txt"
        /// 
        /// ShortenPath(@"\\myserver\theshare\myproject\www_myproj\App_Data\theextremelylongfilename_morelength.txt", 30)
        /// Result: "\\myserver\...e_morelength.txt"
        /// 
        /// ShortenPath(@"\\myserver\theshare\myproject\www_myproj\App_Data\themegafile.txt", 50)
        /// Result: "\\myserver\theshare\...\App_Data\themegafile.txt"
        /// 
        /// ShortenPath(@"\\192.168.1.178\theshare\myproject\www_myproj\App_Data\themegafile.txt", 50)
        /// Result: "\\192.168.1.178\theshare\...\themegafile.txt"
        /// 
        /// ShortenPath(@"\theshare\myproject\www_myproj\App_Data\", 30)
        /// Result: "\theshare\...\App_Data\"
        /// 
        /// ShortenPath(@"\theshare\myproject\www_myproj\App_Data\themegafile.txt", 35)
        /// Result: "\theshare\...\themegafile.txt"
        /// </example>
        public static string ShortenPath(string path, int maxLength)
        {
            var ellipsisChars = "...";
            var dirSeperatorChar = Path.DirectorySeparatorChar;
            var directorySeperator = dirSeperatorChar.ToString();

            //simple guards
            if (path.Length <= maxLength)
            {
                return path;
            }
            var ellipsisLength = ellipsisChars.Length;
            if (maxLength <= ellipsisLength)
            {
                return ellipsisChars;
            }


            //alternate between taking a section from the start (firstPart) or the path and the end (lastPart)
            var isFirstPartsTurn = true; //drive letter has first priority, so start with that and see what else there is room for

            //vars for accumulating the first and last parts of the final shortened path
            var firstPart = "";
            var lastPart = "";
            //keeping track of how many first/last parts have already been added to the shortened path
            var firstPartsUsed = 0;
            var lastPartsUsed = 0;

            var pathParts = path.Split(dirSeperatorChar);
            foreach (string t in pathParts)
            {
                if (isFirstPartsTurn)
                {
                    var partToAdd = pathParts[firstPartsUsed] + directorySeperator;
                    if ((firstPart.Length + lastPart.Length + partToAdd.Length + ellipsisLength) > maxLength)
                    {
                        break;
                    }
                    firstPart = firstPart + partToAdd;
                    if (partToAdd == directorySeperator)
                    {
                        //this is most likely the first part of and UNC or relative path 
                        //do not switch to lastpart, as these are not "true" directory seperators
                        //otherwise "\\myserver\theshare\outproject\www_project\file.txt" becomes "\\...\www_project\file.txt" instead of the intended "\\myserver\...\file.txt")
                    }
                    else
                    {
                        isFirstPartsTurn = false;
                    }
                    firstPartsUsed++;
                }
                else
                {
                    var index = pathParts.Length - lastPartsUsed - 1; //-1 because of length vs. zero-based indexing
                    var partToAdd = directorySeperator + pathParts[index];
                    if ((firstPart.Length + lastPart.Length + partToAdd.Length + ellipsisLength) > maxLength)
                    {
                        break;
                    }
                    lastPart = partToAdd + lastPart;
                    if (partToAdd == directorySeperator)
                    {
                        //this is most likely the last part of a relative path (e.g. "\websites\myproject\www_myproj\App_Data\")
                        //do not proceed to processing firstPart yet
                    }
                    else
                    {
                        isFirstPartsTurn = true;
                    }
                    lastPartsUsed++;
                }
            }

            if (lastPart == "")
            {
                //the filename (and root path) in itself was longer than maxLength, shorten it
                lastPart = pathParts[pathParts.Length - 1];//"pathParts[pathParts.Length -1]" is the equivalent of "Path.GetFileName(pathToShorten)"
                lastPart = lastPart.Substring(lastPart.Length + ellipsisLength + firstPart.Length - maxLength, maxLength - ellipsisLength - firstPart.Length);
            }

            return firstPart + ellipsisChars + lastPart;
        }


        public class PathHierarchy
        {
            private List<string> _pathHierarchy;

            // private List<string> _pathHierarchy;
            public string Path { get { return PathFromHierarchy(); } set { HierarchyFromPath(value); } }
            public List<string> Hierarchy { get { return _pathHierarchy; } set { _pathHierarchy = value; } }

            public PathHierarchy(string path = "")
            {
                _pathHierarchy = new List<string>();
                HierarchyFromPath(path);
            }

            string PathFromHierarchy()
            {
                var s = string.Join("\\", _pathHierarchy);
                return s;

            }

            void HierarchyFromPath(string path)
            {
                _pathHierarchy = path.Split('\\').ToList();
            }
        };

        /*-----------------*/
    }
}
