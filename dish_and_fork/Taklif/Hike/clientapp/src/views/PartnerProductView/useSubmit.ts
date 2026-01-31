import { useCallback } from 'react';

import { MerchandiseCreateModel, MerchandiseUnitType } from '~/types';
import { getNormalizedSaveUnit } from '~/utils';

import { FormValues } from './types';

type OnSubmit = (value: MerchandiseCreateModel) => void;
type UseSubmit = (onSubmit: OnSubmit) => (formData: FormValues) => void;

const useSubmit: UseSubmit = (onSubmit) =>
  useCallback(
    ({ images, ...data }: FormValues) =>
      onSubmit({
        ...data,
        servingSize: getNormalizedSaveUnit(data.servingSize, data.unitType),
        categories: [...data.categories, ...data.compositionCategories],
        servingGrossWeightInKilograms: getNormalizedSaveUnit(
          data.servingGrossWeightInKilograms,
          MerchandiseUnitType.Kilograms,
        ),
        images: images.map((image: any) => (typeof image === 'string' ? image : image.id)),
      }),
    [onSubmit],
  );

export { useSubmit };
export type { OnSubmit, UseSubmit };
