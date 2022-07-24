using System;
using AutoMapper;
using DDD4Tests.Domains;

namespace DDD4Tests.Dtos;

[AutoMap(typeof(Root))]
public class RootDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}