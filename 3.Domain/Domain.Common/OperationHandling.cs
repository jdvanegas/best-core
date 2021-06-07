using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Common
{
  public class Result
  {
    public Result()
    {
      Errors = new List<string>();
    }

    public bool Status => !Errors.Any();

    public List<string> Errors { get; set; }
  }

  public class Result<T> : Result
  {
    public T Data { get; set; }
  }
}