import { Autocomplete, AutocompleteValue, TextField } from '@mui/material';
import { SyntheticEvent, useMemo } from 'react';

import { CategoryReadModel } from '~/types';

const CategoriesChips = ({
  value = [],
  onChange,
  categories,
  title = 'Категории',
}: {
  value?: AutocompleteValue<string, string, string, string> | null;
  onChange?: (selectedCategories: string[]) => void;
  categories: CategoryReadModel[];
  title?: string;
}) => {
  const options = categories.map((category) => {
    return {
      label: category.title,
      id: category.id,
    };
  });

  const categoryIds = useMemo(() => {
    return categories.reduce((acc: Record<string, CategoryReadModel>, value) => {
      acc[value.id] = value;

      return acc;
    }, {});
  }, [categories]);

  const selected = useMemo(() => {
    return value?.map((key) => {
      const item = categoryIds[key];

      return {
        label: item?.title,
        id: item?.id,
      };
    });
  }, [categoryIds, value]);

  const onChangeCallback = (
    _e: SyntheticEvent<Element>,
    values: {
      label: string;
      id: string;
    }[],
  ) => {
    onChange &&
      onChange(
        values.map((value) => {
          return value.id;
        }),
      );
  };

  return (
    <Autocomplete
      multiple
      id="tags-standard"
      onChange={onChangeCallback}
      options={options}
      getOptionLabel={(option) => option.label}
      isOptionEqualToValue={(option, value) => {
        return option.id === value.id;
      }}
      value={selected}
      renderInput={(params) => <TextField {...params} variant="standard" label={title} />}
    />
  );
};

export default CategoriesChips;
