import { MerchandisesState } from '~/types/swagger';

const MAP_STATUS_NAME = {
  [MerchandisesState.Created]: 'Не опубликован',
  [MerchandisesState.Published]: 'Опубликован',
  [MerchandisesState.Blocked]: 'Заблокирован',
};

const MAP_STATUS_COLOR = {
  [MerchandisesState.Created]: '#999999',
  [MerchandisesState.Published]: '4CAF50',
  [MerchandisesState.Blocked]: 'red',
};

interface Row {
  id: string;
  photo: string | null | undefined;
  title: string;
  price: number | string;
  state: MerchandisesState;
  createDate: string;
  availableQuantity?: number;
}

export { MAP_STATUS_COLOR, MAP_STATUS_NAME };
export type { Row };
