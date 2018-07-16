Imports System.Text

Module Module1
    Sub Main()
        Dim ListToSort(100) As Double
        Dim NaN() As Double
        Dim NanListLength As Integer = 0
        Dim NaN1 As Double = CreaterandomNan(New Random)
        Dim NaN2 As Double = CreaterandomNan(New Random)
        Dim NaN3 As Double = CreaterandomNan(New Random)
        Dim NegativeInfinity As Double = -1 / 0
        Dim NegativeZero As Double = -0.0D
        Dim NaNAsAString() As String
        Dim Stopwatch As New Stopwatch

        For i = 0 To 95
            ListToSort(i) = (Rnd() + (Rnd() / Rnd()))
        Next
        ListToSort(95) = NegativeZero
        ListToSort(96) = NaN2
        ListToSort(97) = NegativeInfinity
        ListToSort(98) = NaN1
        NaN = {0}
        NaNAsAString = {""}

        PrintPreSort(ListToSort) 'Calls the print subroutine and prints the list before sorting
        Stopwatch.Reset()
        Stopwatch.Start()
        ExcludeNaN(ListToSort, NaN) 'Calls the subroutine to exclude NaNs from the list
        InsertionSort(ListToSort) 'Calls the sort subroutine
        Stopwatch.Stop()
        Console.WriteLine("The time taken to sort is : {0}", Stopwatch.Elapsed)
        PostSortPrint(ListToSort, NaN, NaNAsAString) 'Prints the final list
        Console.Read()

    End Sub

    Public Sub InsertionSort(ByRef listToSort() As Double)
        If listToSort.Length = 0 Or listToSort.Length = 1 Then
            Console.WriteLine("The list needs no sorting as the number of elements is " & listToSort.Length)
        End If
        Dim i As Integer = 1
        Dim ListSize As Double
        Dim n As Integer
        For i = 1 To listToSort.Length - 1
            ListSize = listToSort(i)
            For n = i - 1 To 0 Step -1
                If listToSort(n) <= ListSize Then Exit For
                listToSort(n + 1) = listToSort(n)
            Next
            listToSort(n + 1) = ListSize
        Next
    End Sub
    Public Sub PrintPreSort(ByVal list() As Double)
        Console.WriteLine("The list entered is: ")
        Dim Counter As Integer = 0
        For i = 0 To list.Length - 1
            Counter = i
            PrintListElement(list, Counter)
        Next
        Console.WriteLine()
    End Sub
    Public Sub PostSortPrint(ByVal list() As Double, ByVal nans() As Double, ByRef nanstring() As String)
        Dim Counter As Integer = 0
        Console.WriteLine("Sorted list excluding NaNs; ")
        For i = 0 To list.Length - 1
            Counter = i
            PrintListElement(list, Counter)
        Next
        Console.WriteLine()
        Console.WriteLine("Excluded Nans: ")
        Nansort(nans, nanstring)
        For i = 0 To nanstring.Length - 1
            Counter = i
            Console.WriteLine(nanstring(i))
        Next
    End Sub
    Public Sub ExcludeNaN(ByRef list() As Double, ByRef nan() As Double)
        Dim NanListLength As Integer = 0
        Dim TempListCount As Integer = 0
        Dim i As Integer
        Dim NaNTemp As New List(Of Double)
        Dim TempList As New List(Of Double)
        TempList = list.ToList
        TempListCount = TempList.Count
        Do While i < TempListCount
            If Double.IsNaN(TempList.Item(i)) Then
                NaNTemp.Add(TempList.Item(i))
                TempList.RemoveAt(i) 'If a NaN is found the remove the Nan from the list
                TempListCount = TempList.Count 'The new count is noted
            Else
                i += 1
            End If
        Loop
        list = TempList.ToArray()
        nan = NaNTemp.ToArray()
    End Sub
    Public Sub Nansort(ByRef nan() As Double, ByRef nanString() As String)
        ReDim nanString(nan.Length - 1)
        For i = 0 To nan.Length - 1
            Dim NaNAsNumber = nan(i) 'Converts the Nans to Binary strings
            Dim NaNAsLong As Long = BitConverter.DoubleToInt64Bits(NaNAsNumber)
            Dim NaNAsBinary As String = Convert.ToString(NaNAsLong, 16) ' 16= means base 16
            nanString(i) = NaNAsBinary
        Next
        Dim a As Integer = 1
        Dim currentNaN As String
        Dim n As Integer
        For a = 1 To nanString.Length - 1 ' Uses the sort to sort the Hex Strings
            currentNaN = nanString(a)
            For n = a - 1 To 0 Step -1
                If nanString(n) <= currentNaN Then Exit For
                nanString(n + 1) = nanString(n)
            Next
            nanString(n + 1) = currentNaN
        Next
    End Sub
    Public Sub PrintListElement(ByVal list() As Double, ByVal counter As Integer)
        If list(counter) = 0 Then 'If selected element is 0' 
            Dim CurrentNumber As Double
            CurrentNumber = list(counter)
            Dim NumberAsLong As Long = BitConverter.DoubleToInt64Bits(CurrentNumber)
            Dim NumberInBinary As String = Convert.ToString(NumberAsLong, 2) 'Converts the element into its bit pattern'
            If NumberInBinary.First = "1" Then 'If the first bit in the bit pattern is 0 then the number is -0'
                Console.Write("-0, ")
            Else 'Otherwise the number must be zero'
                Console.Write("0, ")
            End If

        ElseIf list(counter) = Double.NegativeInfinity ' Captures Negative infinity and outputs "Negative Infinity" string instead of -8
            Console.Write("Negative Infinity,")
        ElseIf list(counter) = Double.PositiveInfinity ' Captures Postive infinity and outputs "Positive Infinity" string instead of 8
            Console.Write("Positive Infinity,")
        Else
            Console.Write(list(counter) & ", ") ' else print the number
        End If
    End Sub
    Function CreateRandomNaN(ByVal r As Random)
        Dim NaNChars As String = "0123456789ABCDEF" 'The range of characters that can be chosen
        Dim StringBuilder As New StringBuilder
        Dim NaN As String
        StringBuilder.Append("FFF") ' First 3 characters must be FFF, puts it in the NaN range
        For i As Integer = 3 To 15 ' Builds up the Hex Nan
            Dim idx As Integer = r.Next(0, 16) 'Picks a character from the the string
            StringBuilder.Append(NaNChars.Substring(idx, 1))
        Next
        NaN = StringBuilder.ToString()
        Dim Number = NaN 'Converts the string to Double
        Dim NumberAsLong As Long = Convert.ToInt64(NaN, 16)
        Dim NumberAsDouble As Double = BitConverter.Int64BitsToDouble(NumberAsLong)
        Return NumberAsDouble
    End Function
End Module
