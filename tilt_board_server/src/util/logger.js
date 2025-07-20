const LEVELS = 
{ 
    info: 'INFO', 
    warn: 'WARN', 
    error: 'ERROR' 
};

function log(level, ...args) 
{
  const prefix = `[${new Date().toISOString()}] [${LEVELS[level]}]`;
  console.log(prefix, ...args);
}

module.exports = 
{
  info:     (...args)   => log('info', ...args),
  warning:  (...args)   => log('warn', ...args),
  error:    (...args)   => log('error', ...args),
};