using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ssb_tool
{
    public static class Vdf
    {
        private static String genIndent(int n)
        {
            return new String('\t', n);
        }

        public static StringWriter Convert(StreamReader input)
        {
            return Convert(JObject.Parse(input.ReadToEnd()));
        }

        public static StringWriter Convert(String input)
        {
            return Convert(JObject.Parse(input));
        }
        
        public static StringWriter Convert(JObject input)
        {
            // https://developer.valvesoftware.com/wiki/KeyValues#Value_Types

            StringWriter output = new StringWriter();
            JsonReader reader = input.CreateReader();

            int indent = 0;
            int depth = 0;
            dynamic last = null;

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        // check for wrapping json root
                        if (depth == 0) { depth++; break; };
                        // insert linebreak on further nesting
                        if (last == JsonToken.PropertyName) {
                            output.Write("\n");
                        };

                        output.WriteLine(genIndent(indent) + "{");
                        indent++;
                        depth++;
                        break;
                    case JsonToken.EndObject:
                        depth--;
                        // skip if we reach the closing of the json root
                        if (depth == 0) { break; };

                        indent--;
                        output.WriteLine(genIndent(indent) + "}");
                        break;
                    case JsonToken.PropertyName:
                        output.Write(genIndent(indent) 
                                   + "\"" 
                                   + reader.Value 
                                   + "\""
                        );
                        break;
                    case JsonToken.String:
                        output.WriteLine(" \"" + reader.Value + "\"");
                        break;
                    default:
                        break;
                }

                last = reader.TokenType;
            }

            return output;
        }
    }
}
