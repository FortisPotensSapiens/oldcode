import { MouseEventHandler, useCallback } from 'react';

type UsePreventedClick = (after?: () => void) => MouseEventHandler<HTMLButtonElement>;

const usePreventedClick: UsePreventedClick = (onClick) =>
  useCallback(
    (event) => {
      event.preventDefault();
      event.stopPropagation();

      if (onClick) {
        onClick();
      }
    },
    [onClick],
  );

export default usePreventedClick;
