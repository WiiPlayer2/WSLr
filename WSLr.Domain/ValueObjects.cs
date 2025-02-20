using Vogen;

namespace WSLr.Domain;

[ValueObject<string>]
public partial record ShimBinary;

[ValueObject<byte[]>]
public partial record OutputData;