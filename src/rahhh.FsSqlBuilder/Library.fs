namespace rahhh.FsSqlBuilder

open System


//type ParameterDefinition(name: string, value: obj, ?dbType: Nullable<System.Data.DbType>) =
//    member this.Name = name
//    member this.Value = value

[<CLIMutable>]
type ParameterDefinition = {
    Name: string
    Value: obj
    DbType: Nullable<System.Data.DbType>
    Size: Nullable<int>
}
    
type Expression =
    | Empty
    | SqlCondition of condition: bool * sqlExpression: string * parameters: ParameterDefinition list
    | AndExpression of left: Expression * right: Expression
    | OrExpression of left: Expression * right: Expression
    | NotExpression of Expression
    | ParameterExpresson of ParameterDefinition
    

[<AbstractClass>]    
type Exp() =
    abstract member Expression : Expression with get
    
    [<CompiledName("And")>]
    static member And(left: Exp, right: Exp) = AndExp(left, right)
        
    [<CompiledName("Or")>]    
    static member Or(left: Exp, right: Exp) = OrExp(left, right)
    
    [<CompiledName("Not")>]    
    static member Not(expression: Exp) = NotExp(expression)
        
and
    AndExp(left: Exp, right: Exp) =
        inherit Exp()
        override this.Expression with get() = AndExpression (left.Expression, right.Expression)
and
    OrExp(left: Exp, right: Exp) =
        inherit Exp()
        override this.Expression with get() = OrExpression (left.Expression, right.Expression)
and
    NotExp(expression: Exp) =
        inherit Exp()
        override this.Expression with get() = NotExpression (expression.Expression)
    
type SqlConditionalExp(condition: bool, sqlExpression: string, [<ParamArray>] parameters: ParameterDefinition array) =
        inherit Exp()
        override this.Expression with get() = SqlCondition (condition, sqlExpression, (Array.toList parameters))
        
type ParameterDefinitionBuilder() =
    member this.Parameters = System.Collections.Generic.List<ParameterDefinition>()

type ParameterDefinitionBuilder with
    member this.Varchar(paramName: string, value: string, size: int) =
        this.Parameters.Add({ Name = paramName; Value = value; DbType = Nullable<_>(System.Data.DbType.AnsiString); Size = Nullable<_>(size) })


type ExpBuilder() =
    member this.And(left: Exp, right: Exp) = AndExp(left, right)
    member this.Or(left: Exp, right: Exp) = OrExp(left, right)
    member this.Not(exp: Exp) = NotExp(exp)
    
type ExpBuilder with
    
    member this.SqlCondition(sqlExpression: string, configParam: System.Action<ParameterDefinitionBuilder>) =

        let builder = ParameterDefinitionBuilder()
         
        configParam.Invoke(builder)
        
        SqlConditionalExp(true, sqlExpression, (Seq.toArray builder.Parameters))

    member this.SqlCondition(condition: bool, sqlExpression: string, configParam: System.Action<ParameterDefinitionBuilder>) =

        let builder = ParameterDefinitionBuilder()
        
        if condition then 
            configParam.Invoke(builder)
        
        SqlConditionalExp(condition, sqlExpression, (Seq.toArray builder.Parameters))
    
     
[<RequireQualifiedAccess>]
module Exp =
    
    [<CompiledName("And")>]
    let And left right = AndExpression (left, right)
    
    [<CompiledName("Or")>]    
    let Or left right = OrExpression (left, right)
        
    [<CompiledName("Not")>]
    let Not expr = NotExpression expr
        
