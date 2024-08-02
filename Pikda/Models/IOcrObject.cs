using System;
using System.ComponentModel;
using System.Drawing;

public interface IOcrObject
{
    string LastName { get; set; }
    string FirstName { get; set; }
    DateTime? BirthDate { get; set; }
    Gender Gender { get; set; }
    BloodType BloodType { get; set; }
    string CardNumber { get; set; }
    Image Image { get; set; }

    // Result of the read operation
    bool Result { get; set; }

    // Error message if any
    string ErrorMessage { get; set; }
}

public enum Gender : short
{
    [Description("Non défini")]
    NotDefined,

    [Description("Masculin")]
    Male,

    [Description("Féminin")]
    Female
}

public enum BloodType : short
{
    [Description("")]
    NotDefined,

    [Description("A +")]
    APositive,

    [Description("A -")]
    ANegative,

    [Description("B +")]
    BPositive,

    [Description("B -")]
    BNegative,

    [Description("O +")]
    OPositive,

    [Description("O -")]
    ONegative,

    [Description("AB +")]
    AbPositive,

    [Description("AB -")]
    AbNegative
}

public class OcrObject : IOcrObject
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public BloodType BloodType { get; set; }
    public string CardNumber { get; set; }
    public Image Image { get; set; }
    public bool Result { get; set; }
    public string ErrorMessage { get; set; }
}