using LanguageExt;
using Vogen;

namespace WSLr.Domain;

[ValueObject<string>]
public partial record ShimTarget;

[ValueObject<Arr<byte>>]
public partial record OutputData;