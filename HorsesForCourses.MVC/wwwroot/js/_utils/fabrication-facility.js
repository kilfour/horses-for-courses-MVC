import { parseArguments } from "./parse-arguments.js";

export function htmlList(items, renderItem) {
    const result = [];
    for (const item of items) {
        result.push(renderItem(item));
    }
    return result;
}

export function htmlIndexedList(items, renderItem) {
    const result = [];
    let ix = 0;
    for (const item of items) {
        result.push(renderItem(item, ix));
        ix++;
    }
    return result;
}

export function html(tag, ...args) {
    const { attrs, children } = parseArguments(args);
    const node = document.createElement(tag);
    applyAttributes(node, attrs);
    appendChildren(node, children);
    return node;
}

const booleanProps = new Set(['checked', 'selected', 'disabled']);

function applyAttributes(node, attrs) {
    for (const [key, value] of Object.entries(attrs)) {
        if (key.startsWith('on') && typeof value === 'function') {
            const eventName = key.slice(2).toLowerCase();
            node.addEventListener(eventName, value);
        } else if (key === 'style' && typeof value === 'object') {
            Object.assign(node.style, value);
        } else if (booleanProps.has(key)) {
            node[key] = Boolean(value);
        } else {
            node.setAttribute(key, value);
        }
    }
}

function appendChildren(node, children) {
    for (const child of children.flat()) {
        node.append(child instanceof Node ? child : document.createTextNode(child));
    }
}

export const __only_for_test = { applyAttributes };
