import { CheckboxValueType } from 'antd/es/checkbox/Group';
import { FC, useCallback, useMemo } from 'react';

import { CategoryReadModel } from '~/types';

import { StyledCheckbox, StyledGroup } from './styled';

const StorefrontFiltersGroup: FC<{
  kind: string;
  value: Record<string, string[]>;
  categories: CategoryReadModel[];
  onChange: (data: Record<string, string[]>) => void;
}> = ({ kind, categories, onChange, value }) => {
  const handleChange = useCallback(
    (e: CheckboxValueType[]) => {
      const newValue = { ...value };
      newValue[kind] = e as string[];

      onChange(newValue);
    },
    [kind, onChange, value],
  );

  const valueByKind = useMemo(() => value[kind], [kind, value]);

  return (
    <StyledGroup onChange={handleChange} value={valueByKind}>
      {categories?.map((category, i) => {
        return (
          <StyledCheckbox value={category.id} key={category.id}>
            {category.title}
          </StyledCheckbox>
        );
      })}
    </StyledGroup>
  );
};

export { StorefrontFiltersGroup };
