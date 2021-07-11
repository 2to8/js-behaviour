import { el } from '@/Widget/el';
import { id } from '@/Widget/id';
import { MoreTags, UnityEngine } from 'csharp';
import { $typeof } from 'puerts';
import Tags = MoreTags.Tags;
import Button = UnityEngine.UI.Button;
import UI = UnityEngine.UI;

let Typemap = new Map();
//Typemap[typeof id.btn.pause] = (e: Tags) => new id.btn.pause(e);
Typemap[typeof el.button] = (e: Tags) => e.GetComponent($typeof(Button))
Typemap[typeof el.text] = (e: Tags) => e.GetComponent($typeof(UI.Text))

export default Typemap;
