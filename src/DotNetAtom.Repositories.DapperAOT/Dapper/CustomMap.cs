using System;
using System.Reflection;
using Dapper;

namespace DotNetAtom.Repositories.DapperAOT;

public class CustomMap : SqlMapper.IMemberMap
{
    private readonly SqlMapper.IMemberMap _memberMap;

    public CustomMap(SqlMapper.IMemberMap memberMap, string columnName)
    {
        _memberMap = memberMap;
        ColumnName = columnName;
    }

    public string ColumnName { get; }

    public Type MemberType => _memberMap.MemberType;

    public PropertyInfo Property => _memberMap.Property;

    public FieldInfo Field => _memberMap.Field;

    public ParameterInfo Parameter => _memberMap.Parameter;
}
