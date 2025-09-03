import { html } from '../_utils/fabrication-facility.js';
import { renderItemList } from './item-list.js';

export function renderSkillList(label, buttonLabel, skills, parentElement) {
    return renderItemList(
        label,
        buttonLabel,
        skills,
        () => '',
        (skill, ix) => html('input',
            {
                type: 'text',
                class: 'form-control skill-input',
                name: `Skills[${ix}]`, // <= The Hacky, but important, bit ...
                value: skill,
                placeholder: 'New skill'
            }
        ),
        parentElement);
}