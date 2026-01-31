type JoinPath = (...parts: Array<string | number>) => string;

const joinPath: JoinPath = (...parts) => `/${parts.join('/')}`;

export { joinPath };
