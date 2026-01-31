import { MerchandiseCreateModel } from '~/types';

import { ImageValue } from './Images';

export type FormValues = Omit<MerchandiseCreateModel, 'images'> & { images: ImageValue[] } & {
  availableQuantity?: number;
  compositionCategories: string[];
};
