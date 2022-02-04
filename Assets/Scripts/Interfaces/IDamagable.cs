/*****************************************************************************
// File Name :         IDamageable.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : An interface for objects that have a health amount.
*****************************************************************************/
public interface IDamagable
{
    /// <summary>
    /// Updates the health of this object.
    /// </summary>
    /// <param name="healthMod">The amount this objects health will change by.</param>
    public void UpdateHealth(int healthMod);

    public int HealthAmount();
}
