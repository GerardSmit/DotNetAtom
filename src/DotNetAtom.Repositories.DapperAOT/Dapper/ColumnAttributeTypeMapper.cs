using System.Reflection;
using Dapper;

namespace DotNetAtom.Repositories.DapperAOT;

public class ColumnAttributeTypeMapper : SqlMapper.ITypeMap
{
    private readonly Dictionary<string, string> _columnMap;
    private readonly DefaultTypeMap _defaultMapper;

    public ColumnAttributeTypeMapper(Type type)
    {
        _defaultMapper = new DefaultTypeMap(type);

        _columnMap = type.GetProperties()
            .Select(i => (Property: i, Name: i.GetCustomAttribute<DbValueAttribute>()?.Name))
            .Where(i => i.Name != null)
            .ToDictionary(i => i.Name!, i => i.Property.Name);
    }

    public ConstructorInfo FindConstructor(string[] names, Type[] types)
    {
        return _defaultMapper.FindConstructor(names, types);
    }

    public ConstructorInfo FindExplicitConstructor()
    {
        return _defaultMapper.FindExplicitConstructor();
    }

    public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
    {
        return _defaultMapper.GetConstructorParameter(constructor, columnName);
    }

    public SqlMapper.IMemberMap GetMember(string columnName)
    {
        if (_columnMap.TryGetValue(columnName, out var name))
        {
            var result = _defaultMapper.GetMember(name);

            if (result != null)
            {
                return new CustomMap(result, columnName);
            }
        }

        return _defaultMapper.GetMember(columnName);
    }
}
