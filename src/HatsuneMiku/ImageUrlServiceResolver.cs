using System.Collections.Generic;

namespace HatsuneMiku;

public delegate IEnumerable<string> ImageUrlServiceResolver(ImageType key);