import { MoreTags } from 'csharp';
import React from 'react';
import { BackButtonSystem } from 'Tags/BackButtonSystem';
import { CoinSystem } from 'Tags/CoinSystem';
import { DiamondSystem } from 'Tags/DiamondSystem';
import { GoButtonSystem } from 'Tags/GoButtonSystem';
import { HealthSystem } from 'Tags/HealthSystem';
import { LevelSystem } from 'Tags/LevelSystem';
import { MoneySystem } from 'Tags/MoneySystem';
import { PauseButtonSystem } from 'Tags/PauseButtonSystem';
import { UserLevelSystem } from 'Tags/UserLevelSystem';
import { id } from 'Widget/id';
import { Item } from 'Widget/Item';
import Tags = MoreTags.Tags;

interface tagItem {
    tags: typeof Item[],
    call: (tags: Tags) => JSX.Element,
}

global.gKey = 0;

export const TagMaps: tagItem[] = [
    { tags: [ id.GoButton ], call: t => <GoButtonSystem tags={ t } key={ gKey += 1 }/> },
    { tags: [ id.BackButton ], call: t => <BackButtonSystem tags={ t } key={ gKey += 1 }/> },
    { tags: [ id.Pause ], call: t => <PauseButtonSystem tags={ t } key={ gKey += 1 }/> },
    { tags: [ id.Money ], call: t => <MoneySystem tags={ t } key={ gKey += 1 }/> },
    { tags: [ id.Coin ], call: t => <CoinSystem tags={ t } key={ gKey += 1 }/> },
    { tags: [ id.Diamond ], call: t => <DiamondSystem tags={ t } key={ gKey += 1 }/> },
    { tags: [ id.UserLevel ], call: t => <UserLevelSystem tags={ t } key={ gKey += 1 }/> },
    { tags: [ id.Health ], call: t => <HealthSystem tags={ t } key={ gKey += 1 }/> },
    { tags: [ id.LevelMain ], call: t => <LevelSystem tags={ t } key={ gKey += 1 }/> },
]