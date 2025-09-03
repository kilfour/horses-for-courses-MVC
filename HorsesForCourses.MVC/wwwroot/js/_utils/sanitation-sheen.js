
import { parseArguments } from "./parse-arguments";
import { html } from "./fabrication-facility";

const insertedStyles = new Set();

export function styled(tag, styleObj, htmlFn) {
    const className = generateReadableClassName(styleObj);

    if (!insertedStyles.has(className)) {
        const cssRule = `.${className} { ${styleObjToCss(styleObj)} }`;
        const styleTag = document.getElementById("__fabrication_styles") ||
            Object.assign(document.head.appendChild(document.createElement("style")), { id: "__fabrication_styles" });
        styleTag.sheet.insertRule(cssRule);
        insertedStyles.add(className);
    }

    return function (...args) {
        const { attrs, children } = parseArguments(args);
        const existing = attrs.class || "";
        return htmlFn(tag, { ...attrs, class: `${existing} ${className}`.trim() }, ...children);
    };
}

function generateReadableClassName(styleObj) {
    const parts = Object.entries(styleObj)
        .slice(0, 2) // just a couple keys to keep it short
        .map(([k, v]) =>
            k.replace(/[A-Z]/g, m => "-" + m.toLowerCase()).replace(/[^a-z]/gi, "") + "-" + v.toString().replace(/[^a-z0-9]/gi, "")
        );
    const hash = hashStyleObj(styleObj);
    return `ff-${parts.join("-").slice(0, 32)}-${hash}`;
}

function hashStyleObj(styleObj) {
    const str = JSON.stringify(styleObj);
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
        hash = ((hash << 5) - hash) + str.charCodeAt(i);
        hash |= 0;
    }
    return Math.abs(hash).toString(36).slice(0, 6); // Shorter but still unique-ish
}

function styleObjToCss(styleObj) {
    return Object.entries(styleObj)
        .map(([k, v]) => `${k.replace(/[A-Z]/g, m => "-" + m.toLowerCase())}: ${v};`)
        .join(" ");
}

export const __only_for_test = { parseArguments, styleObjToCss, insertedStyles };