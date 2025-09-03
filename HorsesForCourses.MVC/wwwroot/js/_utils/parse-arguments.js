export function parseArguments(args) {
    let [first, ...rest] = args;
    if (isAttributesObject(first)) {
        return { attrs: first, children: rest };
    }
    return { attrs: {}, children: args };
}

function isAttributesObject(arg) {
    if (!arg) return false;
    if (typeof arg !== 'object') return false;
    if (Array.isArray(arg)) return false;
    if (arg instanceof Node) return false;
    return true;
}