export function isEmtyOrSpaces(str){
  return str===null ||(typeof (str)==='string' && str.match(/^ *$/))
}