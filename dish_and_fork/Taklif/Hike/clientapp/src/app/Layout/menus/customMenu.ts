import { StarPurple500 } from '@mui/icons-material';

import { getIndividualOrdersPath } from '~/routing';

const individualAppItem = {
  icon: StarPurple500,
  id: 'custom app',
  sup: 'Новое',
  title: 'Индивидуальные заказы',
  to: getIndividualOrdersPath(),
};

const customMenu = [individualAppItem];

export { customMenu, individualAppItem };
