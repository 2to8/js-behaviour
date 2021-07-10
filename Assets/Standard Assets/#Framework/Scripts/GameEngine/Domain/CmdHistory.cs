using Sirenix.Utilities;
using System.Collections.Generic;

namespace GameEngine.Domain {

[GlobalConfig("Assets/Resources/Config")]
public class CmdHistory : GlobalConfig<CmdHistory> {

    public List<string> History = new List<string>();

}

}