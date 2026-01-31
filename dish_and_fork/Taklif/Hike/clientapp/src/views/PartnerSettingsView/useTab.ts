import { SyntheticEvent, useCallback, useState } from 'react';

type ChangeTabHander = (event: SyntheticEvent<Element, Event>, value: string) => void;

type UseTab = (initial: string) => [string, ChangeTabHander];

const useTab: UseTab = (initial) => {
  const [state, setState] = useState(initial);

  const changeTab = useCallback<ChangeTabHander>((event, value) => {
    setState(value);
  }, []);

  return [state, changeTab];
};

export { useTab };
export type { ChangeTabHander, UseTab };
