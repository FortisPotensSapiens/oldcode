import { DragEventHandler } from 'react';

const preventEvent: DragEventHandler<HTMLLabelElement> = (event) => {
  event.preventDefault();
  event.stopPropagation();
};

export { preventEvent };
