import { GridValueGetterParams } from '@mui/x-data-grid';

import { PartnerType } from '~/types';
import { getPartnerTypeText } from '~/utils';

type TypeValueGetter = (params: GridValueGetterParams<PartnerType>) => string;

const typeValueGetter: TypeValueGetter = ({ value }) => (value ? getPartnerTypeText(value) : '');

export { typeValueGetter };
