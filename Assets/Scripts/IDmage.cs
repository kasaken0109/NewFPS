using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamage
{
    void AddDamage(float damage);
}

interface IGetDamage
{
    void GetDamage(float damage);
}
