import { useCallback, useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';

type UseMenuState = () => [
  boolean,
  {
    hide: () => void;
    show: () => void;
  },
];

const useMenuState: UseMenuState = () => {
  const [state, setState] = useState(false);
  const location = useLocation();
  const hide = useCallback(() => setState(false), []);
  const show = useCallback(() => setState(true), []);

  useEffect(() => hide(), [hide, location]);

  return [state, { hide, show }];
};

export { useMenuState };
export type { UseMenuState };
