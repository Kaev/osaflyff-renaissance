Put the box's item id as name, a long with .ibd in the end.
Otherwise the box will not be used as a box..

AddBoxItem(itemid, probability, quantity);
- Parameter 1 is item id
- Parameter 2 is probability to get (1 to 100%)
- Parameter 3 is quantity (food/scrolls... amount increases by this)
- Example: AddBoxItem(21,50,1); -- 50% chance to get an wooden sword
- Example2: AddBoxItem(2036,20,10); -- 20% chance to get 10 moonstones

AddPenya(min, max, probability);
- Parameter 1 is minimum penya it can give
- Parameter 2 is maximum penya it can give
- Parameter 3 is probability to get any penya (1-100%)
- Example: AddPenya(300,600,60); -- 60% chance to get 300 to 600 penya

SetEffect(effectid);
- Sets effect GFX thats gona show on you for opening box
- If you do not set effect, default is no effect
- Example: SetEffect(108); -- Makes it show FlyFF.SYS_EXCHAN01 effect on you (like apply setting stat)

SetMaxItems(amount);
- Sets max items that should be extracted.
- If unset, then it will add as many as there is in the file.
- Example: SetMaxItems(1); -- Makes one random item of the items that was gona be added if not maxitems, can even be none!