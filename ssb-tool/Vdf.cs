using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssb_tool
{
    public static class Vdf
    {
        public static void Convert(StreamReader input, StreamWriter output)
        {
            // https://developer.valvesoftware.com/wiki/KeyValues#Value_Types
            using (var reader = new JsonTextReader(input))
            {
                int indent = 0;
                int depth = 0;
                dynamic last = null;
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonToken.StartObject:
                            if (depth == 0)
                            {
                                depth++;
                                break;
                            }
                            if (last == JsonToken.PropertyName)
                            {
                                output.WriteLine("");
                            }
                            output.WriteLine(new String('\t', indent) + "{");
                            indent++;
                            depth++;
                            break;
                        case JsonToken.EndObject:
                            depth--;
                            if (depth == 0)
                            {
                                break;
                            }
                            indent--;
                            output.WriteLine(new String('\t', indent) + "}");
                            break;
                        case JsonToken.PropertyName:
                            output.Write(new String('\t', indent) + "\"" + reader.Value + "\"");
                            break;
                        case JsonToken.String:
                            output.WriteLine(" \"" + reader.Value + "\"");
                            break;
                        default:
                            Console.WriteLine("Unsupported JSON2VDF entity");
                            break;
                    }
                    last = reader.TokenType;
                }
            }
        }
    }
}
