import { useMemo } from 'react';

import { CategoryReadModel, CategoryType } from '~/types';

export const useCategories = (allCategories: CategoryReadModel[] | null | undefined) => {
  return useMemo(() => {
    const categories: CategoryReadModel[] = [];
    const compositionCategories: CategoryReadModel[] = [];

    allCategories?.forEach((category) => {
      if (category.type === CategoryType.Composition) {
        compositionCategories.push(category);
      } else {
        categories.push(category);
      }
    });

    return [categories, compositionCategories];
  }, [allCategories]);
};

export const useCategoriesByKind = (categories: CategoryReadModel[]) => {
  return useMemo(() => {
    return (categories ?? []).reduce((acc: Record<string, CategoryReadModel[]>, value) => {
      if (!acc[value.type]) {
        acc[value.type] = [];
      }

      acc[value.type].push(value);

      return acc;
    }, {});
  }, [categories]);
};

export const useCategorieIdsByKind = (categories: CategoryReadModel[]) => {
  return useMemo(() => {
    return (categories ?? []).reduce((acc: Record<string, string[]>, value) => {
      if (!acc[value.type]) {
        acc[value.type] = [];
      }

      acc[value.type].push(value.id);

      return acc;
    }, {});
  }, [categories]);
};

export const useCategoriesByShowOnMainPage = (categories: CategoryReadModel[] | null | undefined) => {
  return useMemo(() => {
    return categories?.filter((category) => {
      return category.showOnMainPage;
    });
  }, [categories]);
};
