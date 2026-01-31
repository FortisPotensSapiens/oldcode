import { SyntheticEvent, useCallback } from 'react';
import { useFormContext } from 'react-hook-form';

import { OrderVariant } from './types';

type TabsChanger = (event: SyntheticEvent<Element, Event>, value: OrderVariant) => void;
type UseTabs = (initial: OrderVariant, name?: string) => [OrderVariant, TabsChanger];

const useTabs: UseTabs = (initial, name = 'variant') => {
  const { setValue, watch } = useFormContext();
  const variant = watch(name, initial);
  const change = useCallback<TabsChanger>((event, value) => setValue(name, value), [name, setValue]);

  return [variant ?? initial, change];
};

export { useTabs };
