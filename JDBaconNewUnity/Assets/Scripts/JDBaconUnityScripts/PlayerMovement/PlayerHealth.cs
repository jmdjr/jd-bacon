using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealth : MonoBehaviour
{
    #region Variables
	private int totalHealth = 100;
	private int currentHealth = 100;
	private bool isAlive = true;
    #endregion
	
	#region Getters and Setters
	/// <summary>
	/// Gets the current health.
	/// </summary>
	/// <returns>
	/// The current health.
	/// </returns>
	public int GetCurrentHealth()
	{
		return currentHealth;
	}
	
	/// <summary>
	/// Changes the current health.
	/// </summary>
	public void ChangeCurrentHealth(int variable)
	{
		currentHealth = currentHealth + variable > totalHealth 
						? totalHealth : (currentHealth + variable);
		if(currentHealth <= 0)
			isAlive = false;
	}
	
	/// <summary>
	/// Gets the total health.
	/// </summary>
	/// <returns>
	/// The total health.
	/// </returns>
	public int GetTotalHealth()
	{
		return totalHealth;	
	}
	
	/// <summary>
	/// Sets the total health.
	/// </summary>
	/// <param name='new_health'>
	/// New_health.
	/// </param>
	public void SetTotalHealth(int new_health)
	{
		totalHealth = new_health;
	}
	
	/// <summary>
	/// Resets the health to full.
	/// </summary>
	public void HealthToFull()
	{
		currentHealth = totalHealth;
	}
	
	/// <summary>
	/// Kill this instance.
	/// </summary>
	public void HealthToZero()
	{
		currentHealth = 0;
		isAlive = false;	
	}
	
	/// <summary>
	/// Determines whether this instance is alive.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is alive; otherwise, <c>false</c>.
	/// </returns>
	public bool IsAlive()
	{
		return isAlive;	
	}
	
	/// <summary>
	/// Sets the condition to alive.
	/// </summary>
	public void Alive()
	{
		isAlive = true;
	}
	
	/// <summary>
	/// Sets the condition to dead.
	/// </summary>
	public void Dead()
	{
		isAlive = false;	
	}
	#endregion
	
	
	// Display Character Health
	void OnGUI()
	{
		GUI.Label(new Rect(50, 50, 100, 100), "Health: "+currentHealth);
	}
	
	
}


