import { useMemo } from 'react';

import { MerchandiseReadModel, MerchandiseUnitType } from '~/types';
import { getNormalizedServingSizeNumber } from '~/utils';
import { useCategories } from '~/utils/categories';

import { FormValues } from './types';

type UseDefaultValues = (merchandise?: Partial<MerchandiseReadModel>) => FormValues;

const useDefaultValues: UseDefaultValues = (merchandise) => {
  const { categories } = merchandise ?? {};

  const [allCategories, compositionCategories] = useCategories(categories);

  return useMemo(() => {
    const {
      title,
      description,
      unitType,
      price,
      servingSize,
      images = [],
      availableQuantity,
      servingGrossWeightInKilograms,
    } = merchandise ?? {};

    return {
      description: description ?? '',
      images: images ?? [],
      price: price ?? 0,
      servingSize: getNormalizedServingSizeNumber(servingSize, String(unitType)),
      servingGrossWeightInKilograms:
        getNormalizedServingSizeNumber(servingGrossWeightInKilograms, MerchandiseUnitType.Kilograms) ?? 0,
      title: title ?? '',
      unitType: unitType ?? MerchandiseUnitType.Pieces,
      availableQuantity: availableQuantity ?? 0,
      categories: (allCategories ?? [])?.map((category) => {
        return category.id;
      }),
      compositionCategories: (compositionCategories ?? [])?.map((category) => {
        return category.id;
      }),
    };
  }, [allCategories, compositionCategories, merchandise]);
};

export { useDefaultValues };
export type { UseDefaultValues };
