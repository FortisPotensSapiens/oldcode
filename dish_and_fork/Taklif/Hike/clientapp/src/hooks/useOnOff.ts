import { Dispatch, SetStateAction, useCallback, useState } from 'react';

type UseOnOff = (initial?: boolean) => [
  boolean,
  {
    change: Dispatch<SetStateAction<boolean>>;
    off: () => void;
    on: () => void;
    toggle: () => void;
  },
];

const useOnOff: UseOnOff = (initial = false) => {
  const [state, change] = useState(initial);

  const off = useCallback(() => change(false), []);
  const on = useCallback(() => change(true), []);
  const toggle = useCallback(() => change((prev) => !prev), []);

  return [state, { change, off, on, toggle }];
};

export { useOnOff };
export type { UseOnOff };
