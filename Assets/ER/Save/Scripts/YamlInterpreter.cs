// Ignore Spelling: Json Deserialize Yaml
//需要安装 YamlDotNet (yaml解析器)
//using YamlDotNet.Serialization;
/*
namespace ER.Save
{
    public class YamlInterpreter : ISaveInterpreter
    {
        public bool Deserialize(string text, out SaveData saveData)
        {
            Deserializer deserializer = new Deserializer();
            saveData = deserializer.Deserialize<SaveData>(text);
            return true;
        }

        public string Serialize(SaveData data)
        {
            Serializer serializer = new Serializer();
            return serializer.Serialize(data);
        }
    }
}
*/