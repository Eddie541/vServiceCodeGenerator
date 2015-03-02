using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace UtilitiesLib
{
    public static class StringUtilities {
        public static string TrimPathFileName(string filePath, int maxLength) {
            string ellipsis = "...";
            string fName = Path.GetFileName(filePath);
            int pathLength = filePath.Length;            
            int fileNameLength = fName.Length + 3;

            if (fileNameLength > maxLength) {                
                throw new ApplicationException("Filename length plus separator cannot exceed max length");
            } else if (pathLength > maxLength) {               
                
                string extendedPathFileName = ellipsis + fName;              

                int finalPathLength = maxLength - (fileNameLength + 1);
                if (finalPathLength > 0) {
                    filePath = filePath.Remove(finalPathLength);
                    filePath = filePath.Insert(finalPathLength, extendedPathFileName);
                } else {
                    filePath = extendedPathFileName;
                }

            }
            return filePath;
        }

        public static string SafeSubstring(this string wholeString, int startIndex, int length) {
            int wholeStringLength = wholeString.Length;
            if (wholeStringLength < (startIndex + 1)) {
                return "";
            }

            if (wholeStringLength < (startIndex + length)) {
                return wholeString.Substring(startIndex);
            }

            return wholeString.Substring(startIndex, length);
        }
        
        public static string GetExceptionMessage(Exception e, bool includeInnerExceptions) {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("Exception: {0}", e.Message));
            if (includeInnerExceptions) {
                Exception innerException = e.InnerException;
                while (innerException != null) {
                    sb.AppendLine(string.Format("\r\nInner Exception: {0}", innerException.Message));
                    innerException = innerException.InnerException;
                }
            }
            return sb.ToString();
        }

        public static bool GetBoolean(this XmlReader reader, string attributeName, bool defaultValue) {
            string attributeString = reader.GetAttribute(attributeName);
            if (string.IsNullOrEmpty(attributeString)) {
                return defaultValue;
            }
            if (attributeString == "False") {
                return false;
            }
            return true;
        }

        static string numericString = "1234567890";
        public static bool BeginsWithNumeric(this string s) {
            return numericString.Contains(s[0]);
        }

        public static int AsInteger(this string s) {
            if (s.IsNullOrEmpty()) {
                return 0;
            }
            if (s.Trim().Length == 0) {
                return 0;
            }

            return Int32.Parse(s);
        }

        static string upperString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string ToFirstLetterLower(this string s) {
            if (upperString.Contains(s[0])) {
                return string.Format("{0}{1}", s.Substring(0,1).ToLower(), s.Substring(1));
            }
            return s;
        }

		static char [] illegalChars = { '\\', '/', ':', '*', '?', '\'', '<', '>', '|' };
		public static bool ContainsIllegalFileNameCharacters(this string s) {
			bool hasIllegalChars = false;
			foreach (char c in s) {
				if (illegalChars.Contains(c)) {
					hasIllegalChars = true;
					break;
				}
			}
			return hasIllegalChars;
		}

        private static string alphanumerics = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public static bool IsAlphanumeric(this string s) {
            bool isAlphanumeric = true;
            foreach (char c in s) {
                if (alphanumerics.Contains(c) == false) {
                    isAlphanumeric = false;
                    break;
                }
            }
            return isAlphanumeric;


        }

        public static bool IsNullOrEmpty(this string s) {
            if (s == null || s.Trim().Length == 0) {
                return true;
            }
            return false;
        }

        public static string Truncate(this string originalString, int maxLength) {
            if (originalString.Length > maxLength) {
                return originalString.Substring(0, maxLength);
            }
            return originalString;
        }

		public static string FormatPrice (object value) {
			decimal d = 0.00m;
			string rValue = "0.00";
			if (value != null && decimal.TryParse (value.ToString (), out d)) {
				rValue = FormatPrice (d);
			}
			return rValue;
		}

		public static string FormatPrice (decimal value) {
			return String.Format ("{0:F2}", value);
		}

		public static string SplitCamelCase (string camelString) {
            if (AreAllCharactersUpperCase(camelString)) {
                return camelString;
            }
			StringBuilder result = new StringBuilder ();
			StringBuilder word = null;
			int lastPosition = camelString.Length - 1;
			int position = 0;
            int nextPosition = 1;
            foreach (char c in camelString) {
                nextPosition = position + 1;
                char next = char.MinValue;
                if (nextPosition <= lastPosition) {
                    next = camelString[nextPosition];
                }
                if (char.IsUpper(c) && char.IsUpper(next) == false && position < lastPosition) {
                    if (word != null) {
                        result.Append(word.ToString() + " ");
                    }
                    word = new StringBuilder();
                }
                if (word != null) {
                    word.Append(c);
                }
                position++;
            }
			result.Append (word);
			return result.ToString ();
		}

        private static bool AreAllCharactersUpperCase(string s) {
            return s.All(t => char.IsUpper(t));
        }

		public static string StripOutLineFeedsAndCarriageReturns(this string input) {
			StringBuilder result = new StringBuilder();
			foreach (char c in input) {
				int val = (int)(c);				
				if (val == 13 || val == 10) {
					result.Append(' ');
				} else {
					result.Append(c);
				}
			}
			return result.ToString();
		}

		public static List<string> GetWords(string wordString) {

			StringBuilder word = new StringBuilder();
			List<string> wordList = new List<string>();
			foreach (char c in wordString) {
				if (Char.IsWhiteSpace(c) && word.Length > 0) {
					wordList.Add(word.ToString());
					word = new StringBuilder();
				} else if (Char.IsWhiteSpace(c) == false) {
					word.Append(c);
				}
			}

			return wordList;
		}
    }

    public static class Extensions {
        public static string ToCSV(this List<string> theList) {
            StringBuilder sb = new StringBuilder();
            foreach (string s in theList) {
                sb.AppendFormat("{0}, ", s);
            }
            return sb.ToString().TrimEnd(',', ' ');
        }

        public static object ToComparable(this object o) {
            if (o is DBNull) {
                return null;
            }
            return o;
        }

        public static string ToYN(this bool value) {
            if (value){
                return "Y";
            }
            return "N";
        }

        public static string ToYesNo(this bool value) {
            if (value) {
                return "Yes";
            }
            return "No";
        }

        public static string ToTrueFalse(this bool value) {
            if (value) {
                return "True";
            }
            return "False";
        }

    }

}
