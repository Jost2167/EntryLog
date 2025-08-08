using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace EntryLog.Data.Specifications;

// Permite reescribir una expresión específica dentro de un árbol de expresiones
// Hereda de ExpressionVisitor, que es una clase base para recorrer y modificar árboles de expresiones
public class RewriterVisitor : ExpressionVisitor
{
    private readonly Expression _from, _to;
    
    // from : expresión que se desea reemplazar
    // to : expresión que se utilizará como reemplazo
    public RewriterVisitor(Expression from, Expression to)
    {
        _from = from;
        _to = to;
    }
    
    [return: NotNullIfNotNull("node")]
    public override Expression? Visit(Expression? node)
    {
        // Si el nodo actual es igual a la expresión "from", se reemplaza por "to"
        // De lo contrario, se continúa con la visita normal del árbol de expresiones
        return node == _from ? _to : base.Visit(node);
    }
}