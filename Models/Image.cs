using System;
using System.Collections.Generic;

namespace Connect2Gether_API.Models;

public partial class Image
{
    public int Id { get; set; }

    public byte[] Image1 { get; set; } = null!;

    public byte[] Image2 { get; set; } = null!;

    public byte[] Image3 { get; set; } = null!;

    public byte[] Image4 { get; set; } = null!;
}
