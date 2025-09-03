import { html, htmlList, htmlIndexedList } from '../_utils/fabrication-facility.js';

export function renderItemList(label, buttonLabel, items, newItemFunc, renderItemFunc, parentElement) {
    function reRender() { renderItemList(label, buttonLabel, items, newItemFunc, renderItemFunc, parentElement); }
    function addItem() { items.push(newItemFunc()); reRender(); }
    function removeItem(ix) { items.splice(ix, 1); reRender(); }
    if (items.length === 0) items.push(newItemFunc());
    const skillList =
        html('div',
            html('div', { class: 'imb-2 d-flex justify-content-between align-items-center' },
                html('label', { class: 'form-label m-0' }, label),
                html('button', {
                    onclick: () => addItem(),
                    class: 'btn btn-outline-secondary btn-sm'
                }, buttonLabel)
            ),
            htmlIndexedList(items, (item, ix) =>
                html('div', { class: 'input-group mb-2 skill-row' },
                    renderItemFunc(item, ix),
                    html('button', {
                        onclick: () => removeItem(ix),
                        type: 'button',
                        class: 'btn btn-outline-danger remove-skill'
                    }, 'Remove')
                )
            ));
    parentElement.replaceChildren(skillList);
}

export function selectBy(name, current, getValue, getLabel, items) {
    return html('select', { name: name },
        htmlList(items,
            item => html('option', {
                value: getValue(item),
                selected: (current === getValue(item) ? 'selected' : null)
            }, getLabel(item))
        )
    )
}