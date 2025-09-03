import { html, htmlIndexedList } from '../_utils/fabrication-facility.js';

export function renderSkillList(skills, parentElement) {
    if (skills.length === 0) skills = skills.push('');
    const skillList =
        html('div',
            html('div', { class: 'imb-2 d-flex justify-content-between align-items-center' },
                html('label', { class: 'form-label m-0' }, 'Skills'),
                html('button', {
                    onclick: () => {
                        skills.push('');
                        renderSkillList(skills, parentElement);
                    },
                    type: 'button',
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
                        onclick: () => {
                            skills.splice(ix, 1);
                            renderSkillList(skills, parentElement);
                        },
                        type: 'button',
                        class: 'btn btn-outline-danger remove-skill'
                    }, 'Remove')
                )
            ));
    parentElement.replaceChildren(skillList);
}