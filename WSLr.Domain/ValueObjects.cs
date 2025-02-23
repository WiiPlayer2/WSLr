using LanguageExt;
using Vogen;

namespace WSLr.Domain;

[ValueObject<string>]
public partial record ShimTarget;

[ValueObject<bool>]
public partial record ShimFixStreamLineEndings;

[ValueObject<Arr<byte>>]
public partial record OutputData;

[ValueObject<string>]
public partial record OutputPath;
