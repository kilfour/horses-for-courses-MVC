import { html, htmlIndexedList } from '../_utils/fabrication-facility.js';

export function renderSkillList(skills, parentElement) {
    function reRender() { renderSkillList(skills, parentElement); }
    function addSkill() { skills.push(''); reRender(); }
    function removeSkill(ix) { skills.splice(ix, 1); reRender(); }
    if (skills.length === 0) skills.push('');
    const skillList =
        html('div',
            html('div', { class: 'imb-2 d-flex justify-content-between align-items-center' },
                html('label', { class: 'form-label m-0' }, 'Skills'),
                html('button', {
                    onclick: () => addSkill(),
                    class: 'btn btn-outline-secondary btn-sm'
                }, 'Add Skill')
            ),
            htmlIndexedList(skills, (skill, ix) =>
                html('div', { class: 'input-group mb-2 skill-row' },
                    html('input', {
                        type: 'text',
                        class: 'form-control skill-input',
                        name: `Skills[${ix}]`,
                        value: skill,
                        placeholder: 'New skill'
                    }),
                    html('button', {
                        onclick: () => removeSkill(ix),
                        type: 'button',
                        class: 'btn btn-outline-danger remove-skill'
                    }, 'Remove')
                )
            ));
    parentElement.replaceChildren(skillList);
}