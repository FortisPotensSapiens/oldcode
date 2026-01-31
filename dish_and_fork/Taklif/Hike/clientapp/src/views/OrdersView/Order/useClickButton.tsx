import { MouseEventHandler, useCallback } from 'react';

type UseClickButton = (action: () => void) => MouseEventHandler<HTMLButtonElement>;

const useClickButton: UseClickButton = (action) =>
  useCallback(
    (event) => {
      event.preventDefault();
      event.stopPropagation();

      action();
    },
    [action],
  );

export { useClickButton };
