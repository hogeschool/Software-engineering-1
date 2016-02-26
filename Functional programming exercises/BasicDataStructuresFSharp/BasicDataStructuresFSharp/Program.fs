//A fraction is made of a pair of integer numbers called numerator and denominator.
//The sum of two fractions can be done only if the denominator is the same. The sum is a fraction having the sum of the numerators and the same denominator.
//The multiplication of two fractions is a fraction containing the result of the multiplication of numerators and denominators.
//The division is the multiplication of the first fraction with the inverse of the second.
//Write a function which implements the operation between two fractions. Use a record to model a fraction.

type Fraction =
  {
    Numerator     : int
    Denominator   : int
  }
  with 
    static member Create(a,b) =
      {
        Numerator = a
        Denominator = b
      }
    static member (+) (f1 : Fraction,f2 : Fraction) =
      if f1.Denominator = f2.Denominator then
        Fraction.Create(f1.Numerator + f2.Numerator, f1.Denominator)
      else
        failwith "The two fractions must have the same denominator"

    static member (-) (f1,f2) = f1 + (Fraction.Create(-f2.Numerator,f2.Denominator))

    static member (*) (f1,f2) =
      Fraction.Create(f1.Numerator * f2.Numerator, f1.Denominator * f2.Denominator)

    static member (!) f =
      Fraction.Create(f.Denominator,f.Numerator)

    static member (/) (f1,f2) = f1 * (!f2)

    override this.ToString() = (this.Numerator |> string) + "\n-----------\n" + (this.Denominator |> string)

let (.|) a b = Fraction.Create(a,b)

[<EntryPoint>]
let main argv =
  let f1 = 5 .| 3
  let f2 = 2 .| 4
  let r = f1 * f2
  printfn "%s" (r.ToString())
  0 // return an integer exit code
